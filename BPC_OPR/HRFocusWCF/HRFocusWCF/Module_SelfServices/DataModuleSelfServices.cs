using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ClassLibrary_BPC.hrfocus.model;
namespace BPC_OPR
{
    public class DataModuleSelfServices
    {
    }
    #region InputTRTimeleave
    [DataContract]
    public class InputTRTimeleave
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string worker_name { get; set; }
        [DataMember]
        public int timeleave_id { get; set; }
        [DataMember]
        public string timeleave_doc { get; set; }
        [DataMember]
        public string timeleave_fromdate { get; set; }
        [DataMember]
        public string timeleave_todate { get; set; }
        [DataMember]
        public string timeleave_type { get; set; }
        [DataMember]
        public int timeleave_min { get; set; }
        [DataMember]
        public int timeleave_actualday { get; set; }
        [DataMember]
        public bool timeleave_incholiday { get; set; }
        [DataMember]
        public bool timeleave_deduct { get; set; }
        [DataMember]
        public string timeleave_note { get; set; }
        [DataMember]
        public string leave_code { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public string leave_data { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string year_code { get; set; }
    }
    #endregion  

    #region InputTRTimeot
    [DataContract]
    public class InputTRTimeot
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string worker_name { get; set; }
        [DataMember]
        public int timeot_id { get; set; }
        [DataMember]
        public string timeot_doc { get; set; }
        [DataMember]
        public string timeot_workdate { get; set; }
        [DataMember]
        public int timeot_beforemin { get; set; }
        [DataMember]
        public int timeot_normalmin { get; set; }
        [DataMember]
        public int timeot_aftermin { get; set; }
        [DataMember]
        public string timeot_note { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public string timeot_todate { get; set; }
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public string ot_data { get; set; }
    }
    #endregion

    #region InputTRTimeshift
    [DataContract]
    public class InputTRTimeshift
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string worker_name { get; set; }
        [DataMember]
        public int timeshift_id { get; set; }
        [DataMember]
        public string timeshift_doc { get; set; }
        [DataMember]
        public string timeshift_workdate { get; set; }
        [DataMember]
        public string timeshift_old { get; set; }
        [DataMember]
        public string timeshift_new { get; set; }
        [DataMember]
        public string timeshift_note { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public string timeshift_fromdate { get; set; }
        [DataMember]
        public string timeshift_todate { get; set; }
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public string timeshift_data { get; set; }
    }
    #endregion

    #region InputTRTimecheckin
    [DataContract]
    public class InputTRTimecheckin
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string timecheckin_id { get; set; }
        [DataMember]
        public string timecheckin_workdate { get; set; }
        [DataMember]
        public string timecheckin_workdate_to { get; set; }
        [DataMember]
        public string timecheckin_time { get; set; }
        [DataMember]
        public string timecheckin_type { get; set; }
        [DataMember]
        public double timecheckin_lat { get; set; }
        [DataMember]
        public double timecheckin_long { get; set; }
        [DataMember]
        public string timecheckin_note { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public string timecheckin_data { get; set; }
    }
    #endregion

    #region InputTRTimedaytype
    [DataContract]
    public class InputTRTimedaytype
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int timedaytype_id { get; set; }
        [DataMember]
        public string timedaytype_doc { get; set; }
        [DataMember]
        public string timedaytype_workdate { get; set; }
        [DataMember]
        public string timedaytype_todate { get; set; }
        [DataMember]
        public string timedaytype_old { get; set; }
        [DataMember]
        public string timedaytype_new { get; set; }
        [DataMember]
        public string timedaytype_note { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public string timedaytype_data { get; set; }
    }
    #endregion

    #region InputTRTimeonsite
    [DataContract]
    public class InputTRTimeonsite
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int timeonsite_id { get; set; }
        [DataMember]
        public string timeonsite_doc { get; set; }
        [DataMember]
        public string timeonsite_workdate { get; set; }
        [DataMember]
        public string timeonstie_todate { get; set; }
        [DataMember]
        public string timeonsite_in { get; set; }
        [DataMember]
        public string timeonsite_out { get; set; }
        [DataMember]
        public string timeonsite_note { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public string timeonsite_data { get; set; }
    }
    #endregion

    #region InputMTWorkflow
    [DataContract]
    public class InputMTWorkflow
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string workflow_id { get; set; }
        [DataMember]
        public string workflow_code { get; set; }
        [DataMember]
        public string workflow_name_th { get; set; }
        [DataMember]
        public string workflow_name_en { get; set; }
        [DataMember]
        public string workflow_type { get; set; }

        [DataMember]
        public int step1 { get; set; }
        [DataMember]
        public int step2 { get; set; }
        [DataMember]
        public int step3 { get; set; }
        [DataMember]
        public int step4 { get; set; }
        [DataMember]
        public int step5 { get; set; }

        [DataMember]
        public int totalapprove { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public List<cls_TRLineapprove> lineapprove_data { get; set; }
    }
    #endregion

    #region InputTRLineapprove
    [DataContract]
    public class InputTRLineapprove
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string workflow_type { get; set; }
        [DataMember]
        public string workflow_code { get; set; }
        [DataMember]
        public string position_level { get; set; }
      
        [DataMember]
        public List<cls_TRLineapprove> lineapprove_data { get; set; }

    }
    #endregion

    #region InputMTArea
    [DataContract]
    public class InputMTArea
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int area_id { get; set; }
        [DataMember]
        public double area_lat { get; set; }
        [DataMember]
        public double area_long { get; set; }
        [DataMember]
        public double area_distance { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public List<cls_TRArea> area_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion

    #region InputMTTopic
    [DataContract]
    public class InputMTTopic
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int topic_id { get; set; }
        [DataMember]
        public string topic_code { get; set; }
        [DataMember]
        public string topic_name_th { get; set; }
        [DataMember]
        public string topic_name_en { get; set; }
        [DataMember]
        public string topic_type { get; set; }
        [DataMember]
        public List<cls_MTTopic> topic_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion

    #region InputMTReqdoc
    [DataContract]
    public class InputMTReqdoc
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int reqdoc_id { get; set; }
        [DataMember]
        public string reqdoc_doc { get; set; }
        [DataMember]
        public string reqdoc_date { get; set; }
        [DataMember]
        public string reqdoc_date_to { get; set; }
        [DataMember]
        public string reqdoc_note { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public List<cls_TRReqempinfo> reqempinfo_data { get; set; }
        [DataMember]
        public List<cls_TRReqdocatt> reqdocatt_data { get; set; }


        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion

    #region InputMTJobtable
    [DataContract]
    public class InputMTJobtable
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int jobtable_id { get; set; }
        [DataMember]
        public string job_id { get; set; }
        [DataMember]
        public string job_type { get; set; }
        [DataMember]
        public string status_job { get; set; }
        [DataMember]
        public int job_nextstep { get; set; }
        [DataMember]
        public string job_date { get; set; }
        [DataMember]
        public string job_date_to { get; set; }
        [DataMember]
        public string job_finishdate { get; set; }
        [DataMember]
        public string workflow_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion

    #region InputMTReqdocument
    [DataContract]
    public class InputMTReqdocument
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int document_id { get; set; }
        [DataMember]
        public string job_id { get; set; }
        [DataMember]
        public string job_type { get; set; }
        [DataMember]
        public string document_name { get; set; }
        [DataMember]
        public string document_type { get; set; }
        [DataMember]
        public string document_path { get; set; }
      
        [DataMember]
        public string create_by { get; set; }
        [DataMember]
        public DateTime create_date { get; set; }

    }
    #endregion

    #region InputMTAccount
    [DataContract]
    public class InputMTAccount
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string account_user { get; set; }
        [DataMember]
        public string account_pwd { get; set; }
        [DataMember]
        public string account_type { get; set; }
        [DataMember]
        public int account_level { get; set; }
        [DataMember]
        public string account_email { get; set; }
        [DataMember]
        public bool account_email_alert { get; set; }
        [DataMember]
        public string account_line { get; set; }
        [DataMember]
        public bool account_line_alert { get; set; }

        [DataMember]
        public List<cls_TRAccountpos> positonn_data { get; set; }
        [DataMember]
        public List<cls_TRAccountdep> dep_data { get; set; }
        [DataMember]
        public List<cls_TRAccount> worker_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

        [DataMember]
        public string workflow_type { get; set; }
        [DataMember]
        public string worker_code { get; set; }

    }
    #endregion

    #region InputTRAccountpos
    [DataContract]
    public class InputTRAccountpos
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string account_user { get; set; }
        [DataMember]
        public string account_type { get; set; }
        [DataMember]
        public string position_code { get; set; }
    }
    #endregion

    #region InputTRAccountdep
    [DataContract]
    public class InputTRAccountdep
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string account_user { get; set; }
        [DataMember]
        public string account_type { get; set; }
        [DataMember]
        public string level_code { get; set; }
        [DataMember]
        public string dep_code { get; set; }
    }
    #endregion

}