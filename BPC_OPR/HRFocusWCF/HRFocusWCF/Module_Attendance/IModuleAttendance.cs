
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using ClassLibrary_BPC.hrfocus.model;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
namespace BPC_OPR
{
    [ServiceContract]
    public interface IModuleAttendance
    {

        #region MTPeriod
        //[OperationContract(Name = "period_list")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string getMTPeriodList(InputMTPeriod input);

        //[OperationContract(Name = "period")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doManageMTPeriod(InputMTPeriod input);

        //[OperationContract(Name = "period_del")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string doDeleteMTPeriod(InputMTPeriod input);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/doUploadMTPeriod?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        //Task<string> doUploadMTPeriod(string token, string by, string fileName, Stream stream,string com);
        #endregion

        #region MTPlanholiday
        [OperationContract(Name = "planholiday_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTPlanholidayList(InputMTPlanholiday input);

        [OperationContract(Name = "planholiday")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTPlanholiday(InputMTPlanholiday input);

        [OperationContract(Name = "planholiday_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTPlanholiday(InputMTPlanholiday input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTPlanholiday?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTPlanholiday(string token, string by, string fileName, Stream stream,string com);
        #endregion

        #region MTShift
        [OperationContract(Name = "shift_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTShiftList(InputMTShift input);

        [OperationContract(Name = "shift")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTShift(InputMTShift input);

        [OperationContract(Name = "shift_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTShift(InputMTShift input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTShift?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTShift(string token, string by, string fileName, Stream stream,string com);

        #endregion

        #region MTPlanshift
        [OperationContract(Name = "planshift_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTPlanshiftList(InputMTPlanshift input);

        [OperationContract(Name = "planshift")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTPlanshift(InputMTPlanshift input);

        [OperationContract(Name = "planshift_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTPlanshift(InputMTPlanshift input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTPlanshift?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTPlanshift(string token, string by, string fileName, Stream stream,string com);

        #endregion

        #region MTLeave
        [OperationContract(Name = "leave_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTLeaveList(InputMTLeave input);

        [OperationContract(Name = "leave")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTLeave(InputMTLeave input);

        [OperationContract(Name = "leave_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTLeave(InputMTLeave input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTLeave?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTLeave(string token, string by, string fileName, Stream stream,string com);

        #endregion

        #region TRLeave
        [OperationContract(Name = "leaveacc_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTReaveList(InputTRLeave input);

        [OperationContract(Name = "leaveacc")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTReave(InputTRLeave input);

        [OperationContract(Name = "leaveacc_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTReave(InputTRLeave input);
        #endregion

        #region MTPlanleave
        [OperationContract(Name = "planleave_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTPlanleaveList(InputMTPlanleave input);

        [OperationContract(Name = "planleave")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTPlanleave(InputMTPlanleave input);

        [OperationContract(Name = "planleave_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTPlanleave(InputMTPlanleave input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTPlanleave?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTPlanleave(string token, string by, string fileName, Stream stream,string com);

        #endregion

        #region MTRateot
        [OperationContract(Name = "ot_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTRateotList(InputMTRateot input);

        [OperationContract(Name = "ot")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTRateot(InputMTRateot input);

        [OperationContract(Name = "ot_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTRateot(InputMTRateot input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTRateot?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTRateot(string token, string by, string fileName, Stream stream,string com);

        #endregion

        #region MTDiligence
        [OperationContract(Name = "diligence_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTDiligenceList(InputMTDiligence input);

        [OperationContract(Name = "diligence")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTDiligence(InputMTDiligence input);

        [OperationContract(Name = "diligence_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTDiligence(InputMTDiligence input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTDiligence?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTDiligence(string token, string by, string fileName, Stream stream,string com);

        #endregion

        #region MTLate
        [OperationContract(Name = "late_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTLateList(InputMTLate input);

        [OperationContract(Name = "late")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTLate(InputMTLate input);

        [OperationContract(Name = "late_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTLate(InputMTLate input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTLate?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTLate(string token, string by, string fileName, Stream stream,string com);

        #endregion

        #region MTPlantimeallw
        [OperationContract(Name = "plantimeallw_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTPlantimeallwList(InputMTPlantimeallw input);

        [OperationContract(Name = "plantimeallw")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTPlantimeallw(InputMTPlantimeallw input);

        [OperationContract(Name = "plantimeallw_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTPlantimeallw(InputMTPlantimeallw input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTPlantimeallw?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTPlantimeallw(string token, string by, string fileName, Stream stream,string com);

        #endregion

        #region SetPolicyAtt
        [OperationContract(Name = "SetPolicyAtt_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getPolicyAttendance(InputSetPolicyAtt input);

        [OperationContract(Name = "SetPolicyAtt")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetPolicyAttendance(InputSetPolicyAtt input);

        [OperationContract(Name = "SetPolicyAtt_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeletePolicyAttendance(InputSetPolicyAtt input);

        #endregion

        #region SetBatchPlanshift
        [OperationContract(Name = "SetBatchPlanshift")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetBatchPlanshift(InputBatchPlanshift input);
        #endregion

        #region Timecard
        [OperationContract(Name = "timecard_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRTimecardList(FillterAttendance req);

        [OperationContract(Name = "timecard")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRTimecard(InputTRTimecard input);

        [OperationContract(Name = "timesheet")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRTimesheet(InputTRTimecard input);

        [OperationContract(Name = "daytype_list")]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getDaytype();


        [OperationContract(Name = "timecard_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRTimecard(InputTRTimecard input);

        #endregion

        #region Timeimport
        [OperationContract(Name = "timeformat_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTTimeimpformatList(FillterAttendance req);

        [OperationContract(Name = "timeformat")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTTimeimpformat(InputMTTimeimpformat input);

        [OperationContract(Name = "timeformat_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTTimeimpformat(InputMTTimeimpformat input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doReadSimpleTimeInput?fileName={fileName}", ResponseFormat = WebMessageFormat.Json)]
        string doReadSimpleTimeInput(string fileName, Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTimeInput?fileName={fileName}", ResponseFormat = WebMessageFormat.Json)]
        string doUploadTimeInput(string fileName, Stream stream);
        #endregion

        #region Set Batch Attendance Item
        [OperationContract(Name = "polattpay_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getPolicyAttendanceItem(InputSetPolicyAttItem input);

        [OperationContract(Name = "polattpays")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doSetPolicyAttendanceItem(InputSetPolicyAttItem input);

        [OperationContract(Name = "polattpay_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTREmppolattItem(InputSetPolicyAttItem input);
        #endregion

        #region Wageday
        [OperationContract(Name = "wageday_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRWagedayList(FillterAttendance req);
        #endregion

        #region  Lost Wages
        [OperationContract(Name = "Lostwages_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRLostwagesList(FillterAttendance req);

        [OperationContract(Name = "Lostwages")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRLostwages(InputTRLostwages input);

        [OperationContract(Name = "timesheet1")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRTimesheetLostwages(InputTRLostwages input);

        [OperationContract(Name = "daytype1_list")]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getDaytype1();


        [OperationContract(Name = "Lostwages_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRLostwages(InputTRLostwages input);

        #endregion
    }

}
