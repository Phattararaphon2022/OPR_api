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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IModuleRecruitment" in both code and config file together.
    [ServiceContract]
    public interface IModuleRecruitment
    {
        #region applywork
        //[OperationContract(Name = "applywork_list")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string getApplyworkList(FillterApplywork req);

        //[OperationContract(Name = "applywork")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doManageMTApplywork(InputMTApplywork input);

        //[OperationContract(Name = "applywork_del")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doDeleteMTApplywork(InputMTApplywork input);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/doUploadApplywork?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        //Task<string> doUploadApplywork(string token, string by, string fileName, Stream stream);

        //recruit
        [OperationContract(Name = "reqworker_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getReqWorkerList(FillterApplywork req);

        [OperationContract(Name = "reqworker")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageReqWorker(InputReqWorker input);

        [OperationContract(Name = "reqworker_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteReqWorker(InputReqWorker input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReqworker?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadReqworker(string token, string by, string fileName, Stream stream,string com);

        [OperationContract(Name = "requpdate_status")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doUpdateStatusReq(InputReqWorker input);

        //image
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReqImages?ref_to={ref_to}", ResponseFormat = WebMessageFormat.Json)]
        string doUploadReqImages(string ref_to, Stream stream);

        [OperationContract(Name = "reqimages")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doGetReqImages(FillterApplywork req);

        [OperationContract(Name = "images_req")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getReqImageList(FillterApplywork req);

        [OperationContract(Name = "doreqimages")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageReqImage(InputTRReqImage input);
        #endregion

        #region TR_Address
        [OperationContract(Name = "reqadd_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRApplyAddressList(FillterApplywork input);

        [OperationContract(Name = "reqaddress")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRApplyAddress(InputApplyTransaction input);

        [OperationContract(Name = "reqadd_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRApplyAddress(InputTRApplyAddress input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadApplyAddress?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadApplyAddress(string token, string by, string fileName, Stream stream, string com);

        #endregion


        #region TR_CARD
        [OperationContract(Name = "reqcardlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRApplyCardList(FillterApplywork input);

        [OperationContract(Name = "reqcard")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRApplyCard(InputApplyTransaction input);

        [OperationContract(Name = "reqcard_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRApplyCard(InputTRApplyCard input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadApplyCard?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadApplyCard(string token, string by, string fileName, Stream stream,string com);
        #endregion

        #region TR_Foreigner
        [OperationContract(Name = "reqforeignerlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRForeignerList(FillterApplywork input);
        [OperationContract(Name = "reqforeigner")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRreqForeigner(InputTRReqForeigner input);
        [OperationContract(Name = "reqforeigner_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRreqForeigner(InputTRReqForeigner input);
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadForeigner?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadreqForeigner(string token, string by, string fileName, Stream stream,string com);

        //--
        [OperationContract(Name = "reqforeignercardlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRForeignercardList(FillterApplywork input);

        [OperationContract(Name = "reqforeignercard")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRForeignercard(InputWorkerTransaction input);

        [OperationContract(Name = "reqforeignercard_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRForeignercard(InputTRReqForeigner input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadreqForeignercard?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadreqForeignercard(string token, string by, string fileName, Stream stream, string com);
        #endregion


        #region Education
        [OperationContract(Name = "reqeducationlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRreqEducationList(FillterApplywork input);

        [OperationContract(Name = "reqeducation")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRreqEducation(InputApplyTransaction input);

        [OperationContract(Name = "reqeducation_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRreqEducation(InputTRReqEducation input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadreqEducation?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadreqEducation(string token, string by, string fileName, Stream stream,string com);
        #endregion

        #region Training
        [OperationContract(Name = "reqtraininglist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRreqTrainingList(FillterApplywork input);

        [OperationContract(Name = "reqtraining")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRreqTraining(InputApplyTransaction input);

        [OperationContract(Name = "reqtraining_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRreqTraining(InputTRReqTraining input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadreqTraining?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadreqTraining(string token, string by, string fileName, Stream stream,string com);
        #endregion

        #region TR_Assessment
        [OperationContract(Name = "reqassessmentlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRAssessmentList(FillterApplywork input);

        [OperationContract(Name = "reqassessment")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRAssessment(InputApplyTransaction input);

        [OperationContract(Name = "reqassessment_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRAssessment(InputTRReqAssessment input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReqAssessment?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadReqAssessment(string token, string by, string fileName, Stream stream,string com);
        #endregion

        #region TR_Criminal
        [OperationContract(Name = "reqcriminallist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRCriminalList(FillterApplywork input);

        [OperationContract(Name = "reqcriminal")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRCriminal(InputApplyTransaction input);

        [OperationContract(Name = "reqcriminal_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRCriminal(InputTRReqCriminal input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReqCriminal?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadReqCriminal(string token, string by, string fileName, Stream stream, string com);
        #endregion

        #region TR_Suggest
        [OperationContract(Name = "reqsuggestlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTReqsuggestList(FillterApplywork input);

        [OperationContract(Name = "reqsuggest")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRReqsuggest(InputApplyTransaction input);

        [OperationContract(Name = "reqsuggest_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRreqSuggest(InputTRReqSuggest input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadreqSuggest?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadreqSuggest(string token, string by, string fileName, Stream stream, string com);
        #endregion

        #region Blacklist
        [OperationContract(Name = "getblacklist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTBlacklistList(InputBlacklist input);

        [OperationContract(Name = "blacklist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTBlacklistList(InputBlacklist input);

        [OperationContract(Name = "blacklist_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTBlacklistList(InputBlacklist input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTBlacklistList?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTBlacklistList(string token, string by, string fileName, Stream stream,string com);

        [OperationContract(Name = "batchblacklist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchBlacklist(InputBlacklist input);
        #endregion

        #region Attach File
        [OperationContract(Name = "docatt_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTReqDocattList(FillterApplywork input);

        [OperationContract(Name = "docatt")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRReqDocatt(InputApplyTransaction input);

        [OperationContract(Name = "docatt_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRReqDocatt(InputTRDocatt input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReqDocatt?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadReqDocatt(string token, string by, string fileName, Stream stream,string com);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTDocatt?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTDocatt(string token, string by, string fileName, Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doGetMTDocatt?file_Path={file_path}", ResponseFormat = WebMessageFormat.Json)]
        byte[] DownloadFile(string file_Path);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doDeleteMTDocatt?file_Path={file_path}", ResponseFormat = WebMessageFormat.Json)]
        string DeleteFile(string file_Path);
        #endregion

        #region TR_POSITION
        [OperationContract(Name = "reqpsotionlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRApplyPositionList(FillterApplywork input);

        [OperationContract(Name = "reqposition")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRApplyPosition(InputApplyTransaction input);

        [OperationContract(Name = "reqposition_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRApplyPosition(InputTRApplyPosition input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadApplyPosition?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadApplyPosition(string token, string by, string fileName, Stream stream,string com);
        #endregion

        #region TR_PROJECT
        [OperationContract(Name = "reqprojectlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRApplyProjectList(FillterApplywork input);

        [OperationContract(Name = "reqproject")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRApplyProject(InputApplyTransaction input);

        [OperationContract(Name = "reqproject_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRApplyProject(InputTRApplyProject input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadApplyProject?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadApplyProject(string token, string by, string fileName, Stream stream,string com);
        #endregion

        #region TR_Salary
        [OperationContract(Name = "reqsalarylist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRApplySalaryList(FillterApplywork input);

        [OperationContract(Name = "reqsalary")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRApplySalary(InputApplyTransaction input);

        [OperationContract(Name = "reqsalary_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRApplySalary(InputTRApplySalary input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadApplySalary?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadApplySalary(string token, string by, string fileName, Stream stream, string com);
        #endregion

        #region TR_Benefit
        [OperationContract(Name = "reqbenefitlist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRApplyBenefitList(FillterApplywork input);

        [OperationContract(Name = "reqbenefit")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRApplyBenefit(InputApplyTransaction input);

        [OperationContract(Name = "reqbenefit_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRApplyBenefit(InputTRApplyBenefit input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadApplyBenefit?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadApplyBenefit(string token, string by, string fileName, Stream stream,string com);
        #endregion

        #region ReqRequest
        [OperationContract(Name = "getreqrequest")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTReqRequest(InputReqRequest input);

        [OperationContract(Name = "reqrequest")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTReqRequestList(InputReqRequest input);

        [OperationContract(Name = "reqrequest_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTReqRequestList(InputReqRequest input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTReqRequestList?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTReqRequestList(string token, string by, string fileName, Stream stream,string com);

        [OperationContract(Name = "getrequestposition")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTReqRequestPosition(InputReqRequest input);

        [OperationContract(Name = "getrequestproject")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTReqRequestProject(InputReqRequest input);

        [OperationContract(Name = "request_status")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doUpdateStatusRequest(InputReqRequest input);
        #endregion

        #region TR_Experience
        [OperationContract(Name = "reqexperiencelist")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRReqExperienceList(FillterApplywork input);

        [OperationContract(Name = "reqexperience")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRReqExperience(InputApplyTransaction input);

        [OperationContract(Name = "reqexperience_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRReqExperience(InputTRReqExperience input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReqExperience?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadReqExperience(string token, string by, string fileName, Stream stream, string com);
        #endregion

    }

}
