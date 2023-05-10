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
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReqworker?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadReqworker(string token, string by, string fileName, Stream stream);
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
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadApplyAddress?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadApplyAddress(string token, string by, string fileName, Stream stream);

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
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadApplyCard?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadApplyCard(string token, string by, string fileName, Stream stream);
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
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadForeigner?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadreqForeigner(string token, string by, string fileName, Stream stream);
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
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadreqEducation?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadreqEducation(string token, string by, string fileName, Stream stream);
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
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadreqTraining?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadreqTraining(string token, string by, string fileName, Stream stream);
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
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReqAssessment?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadReqAssessment(string token, string by, string fileName, Stream stream);
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
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadReqCriminal?fileName={fileName}&token={token}&by={by}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadReqCriminal(string token, string by, string fileName, Stream stream);
        #endregion
     
    }

}
