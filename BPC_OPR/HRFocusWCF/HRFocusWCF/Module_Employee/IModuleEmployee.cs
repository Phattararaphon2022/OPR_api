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
        string getWorkerList(FillterWorker req);

        [OperationContract(Name = "worker")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTWorker(InputMTWorker input);

        [OperationContract(Name = "worker_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTWorker(InputMTWorker input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadWorker?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadWorker(string token, string by, string fileName, Stream stream);

        [OperationContract(Name = "worker_listbyfillter")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getWorkerFillterList(FillterSearch req);

        //image
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadWorkerImages?ref_to={ref_to}", ResponseFormat = WebMessageFormat.Json)]
        string doUploadWorkerImages(string ref_to, Stream stream);

        [OperationContract(Name = "doGetWorkerImages")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doGetWorkerImages(FillterWorker req);
        #endregion

        #region Dep
        [OperationContract(Name = "dep_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getDepList(FillterWorker req);

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

        #region TR_Location
        [OperationContract(Name = "emplocationlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTREmpLocationList(FillterWorker input);

        [OperationContract(Name = "emplocation")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTREmpLocation(InputWorkerTransaction input);

        [OperationContract(Name = "emplocation_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTREmpLocation(InputTREmpLocation input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpLocation?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpLocation(string token, string by, string fileName, Stream stream);

        #endregion

        #region TR_Branch
        [OperationContract(Name = "empbranchlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTREmpBranchList(FillterWorker input);

        [OperationContract(Name = "empbranch")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTREmpBranch(InputWorkerTransaction input);

        [OperationContract(Name = "empbranch_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTREmpBranch(InputTREmpBranch input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpBranch?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpBranch(string token, string by, string fileName, Stream stream);

        #endregion

        #region TR_Address
        [OperationContract(Name = "empaddlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRAddressList(FillterWorker input);

        [OperationContract(Name = "empaddress")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRAddress(InputWorkerTransaction input);

        [OperationContract(Name = "empadd_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRAddress(InputTRAddress input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadAddress?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadAddress(string token, string by, string fileName, Stream stream);

        #endregion

        #region TR_CARD
        [OperationContract(Name = "empcardlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRCardList(FillterWorker input);

        [OperationContract(Name = "empcard")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRCard(InputWorkerTransaction input);

        [OperationContract(Name = "empcard_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRCard(InputTRCard input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadCard?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadCard(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_BANK
        [OperationContract(Name = "empbanklist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRBankList(FillterWorker input);

        [OperationContract(Name = "empbank")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRBank(InputWorkerTransaction input);

        [OperationContract(Name = "empbank_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRBank(InputTRBank input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadBank?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadBank(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Family
        [OperationContract(Name = "empfamilylist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRFamilyList(FillterWorker input);

        [OperationContract(Name = "empfamily")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRFamily(InputWorkerTransaction input);

        [OperationContract(Name = "empfamily_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRFamily(InputTREmpfamily input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadFamily?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadFamily(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Hospital
        [OperationContract(Name = "emphospitallist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRHospitalList(FillterWorker input);

        [OperationContract(Name = "emphospital")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRHospital(InputWorkerTransaction input);

        [OperationContract(Name = "emphospital_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRHospital(InputTREmpHospital input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadHospital?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadHospital(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Foreigner
        [OperationContract(Name = "empforeignerlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRForeignerList(FillterWorker input);

        [OperationContract(Name = "empforeigner")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRForeigner(InputTREmpForeigner input);

        [OperationContract(Name = "empforeigner_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRForeigner(InputTREmpForeigner input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadForeigner?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadForeigner(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Dep
        [OperationContract(Name = "empdeplist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRDepList(FillterWorker input);

        [OperationContract(Name = "empdep")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRDep(InputWorkerTransaction input);

        [OperationContract(Name = "empdep_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRDep(InputTREmpDep input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpDep?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpDep(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Position
        [OperationContract(Name = "emppositionlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRPositionList(FillterWorker input);

        [OperationContract(Name = "empposition")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRPosition(InputWorkerTransaction input);

        [OperationContract(Name = "empposition_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRPosition(InputTREmpPosition input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpPosition?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpPosition(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Group
        [OperationContract(Name = "empgrouplist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRGroupList(FillterWorker input);

        [OperationContract(Name = "empgroup")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRGroup(InputWorkerTransaction input);

        [OperationContract(Name = "empgroup_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRGroup(InputTREmpGroup input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpGroup?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpGroup(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Education
        [OperationContract(Name = "empeducationlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTREducationList(FillterWorker input);

        [OperationContract(Name = "empeducation")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTREducation(InputWorkerTransaction input);

        [OperationContract(Name = "empeducation_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTREducation(InputTREmpEducation input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpEducation?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpEducation(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Training
        [OperationContract(Name = "emptraininglist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRTrainingList(FillterWorker input);

        [OperationContract(Name = "emptraining")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRTraining(InputWorkerTransaction input);

        [OperationContract(Name = "emptraining_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRTraining(InputTREmpTraining input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpTraining?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpTraining(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Assessment
        [OperationContract(Name = "empassessmentlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRAssessmentList(FillterWorker input);

        [OperationContract(Name = "empassessment")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRAssessment(InputWorkerTransaction input);

        [OperationContract(Name = "empassessment_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRAssessment(InputTREmpAssessment input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpAssessment?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpAssessment(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Criminal
        [OperationContract(Name = "empcriminallist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRCriminalList(FillterWorker input);

        [OperationContract(Name = "empcriminal")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRCriminal(InputWorkerTransaction input);

        [OperationContract(Name = "empcriminal_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRCriminal(InputTREmpCriminal input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpCriminal?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpCriminal(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Salary
        [OperationContract(Name = "empsalarylist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRSalaryList(FillterWorker input);

        [OperationContract(Name = "empsalary")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRSalary(InputWorkerTransaction input);

        [OperationContract(Name = "empsalary_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRSalary(InputTREmpSalary input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpSalary?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpSalary(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Provident
        [OperationContract(Name = "empprovidentlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProvidentList(FillterWorker input);

        [OperationContract(Name = "empprovident")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProvident(InputWorkerTransaction input);

        [OperationContract(Name = "empprovident_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProvident(InputTREmpProvident input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpProvident?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpProvident(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Benefit
        [OperationContract(Name = "empbenefitlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRBenefitList(FillterWorker input);

        [OperationContract(Name = "empbenefit")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRBenefit(InputWorkerTransaction input);

        [OperationContract(Name = "empbenefit_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRBenefit(InputTREmpBenefit input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpBenefit?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpBenefit(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Reduce
        [OperationContract(Name = "empreducelist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRReduceList(FillterWorker input);

        [OperationContract(Name = "empreduce")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRReduce(InputWorkerTransaction input);

        [OperationContract(Name = "empreduce_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRReduce(InputTREmpReduce input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpReduce?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpReduce(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Accumalate
        [OperationContract(Name = "empaccumalatelist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRAccumalateList(FillterWorker input);
        #endregion

        #region TR_Supply
        [OperationContract(Name = "empsupplylist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRSupplyList(FillterWorker input);

        [OperationContract(Name = "empsupply")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRSupply(InputWorkerTransaction input);

        [OperationContract(Name = "empsupply_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRSupply(InputTREmpSupply input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpSupply?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpSupply(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Uniform
        [OperationContract(Name = "empuniformlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRUniformList(FillterWorker input);

        [OperationContract(Name = "empuniform")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRUniform(InputWorkerTransaction input);

        [OperationContract(Name = "empuniform_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRUniform(InputTREmpUniform input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpUnifrom?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpUnifrom(string token, string by, string fileName, Stream stream);
        #endregion

        #region TR_Suggest
        [OperationContract(Name = "empsuggestlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRSuggestList(FillterWorker input);

        [OperationContract(Name = "empsuggest")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRSuggest(InputWorkerTransaction input);

        [OperationContract(Name = "empsuggest_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRSuggest(InputTREmpSuggest input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadEmpSuggest?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadEmpSuggest(string token, string by, string fileName, Stream stream);
        #endregion

        #region set batch
        //set position
        [OperationContract(Name = "batchpositionlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchPositionList(InputSetPosition input);

        [OperationContract(Name = "setbatchposition")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchPosition(InputSetPosition input);

        //set dep
        [OperationContract(Name = "batchdeplist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchDepList(InputSetDep input);

        [OperationContract(Name = "setbatchdep")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchDep(InputSetDep input);

        //set group
        [OperationContract(Name = "batchgrouplist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchGroupList(InputSetGroup input);

        [OperationContract(Name = "setbatchgroup")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchGroup(InputSetGroup input);

        //set location
        [OperationContract(Name = "batchlocationlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchLocationList(InputSetLocation input);

        [OperationContract(Name = "setbatchlocation")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchLocation(InputSetLocation input);

        //set salary
        [OperationContract(Name = "batchsalarylist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchSalaryList(InputSetSalary input);

        [OperationContract(Name = "setbatchsalary")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchSalary(InputSetSalary input);

        //set provident
        [OperationContract(Name = "batchprovidentlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchProvidentList(InputSetProvident input);

        [OperationContract(Name = "setbatchprovident")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchProvident(InputSetProvident input);

        //set benefits
        [OperationContract(Name = "batchbenefitlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchBenefitsList(InputSetBenefits input);

        [OperationContract(Name = "setbatchbenefit")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchBenefits(InputSetBenefits input);

        //set training
        [OperationContract(Name = "batchtraininglist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getBatchTrainingList(InputSetTraining input);

        [OperationContract(Name = "setbatchtraining")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchTraining(InputSetTraining input);
        #endregion

        #endregion
    }
}
