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
        public string timeleave_id { get; set; }
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
        public string status { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public string leave_data { get; set; }
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
        public string timeot_id { get; set; }
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
        public string status { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public string timeot_fromdate { get; set; }
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
        public string timeshift_id { get; set; }
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
        public string status { get; set; }
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
        public string worker_code { get; set; }
        [DataMember]
        public string lineapprove_id { get; set; }
        [DataMember]
        public string lineapprove_leave { get; set; }
        [DataMember]
        public string lineapprove_ot { get; set; }
        [DataMember]
        public string lineapprove_shift { get; set; }
        [DataMember]
        public string lineapprove_punchcard { get; set; }
        [DataMember]
        public string lineapprove_checkin { get; set; }
        [DataMember]
        public List<cls_TRLineapprove> lineapprove_data { get; set; }

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
        public string account_emp { get; set; }
        [DataMember]
        public string account_email { get; set; }
        [DataMember]
        public bool account_email_alert { get; set; }
        [DataMember]
        public string account_line { get; set; }
        [DataMember]
        public bool account_line_alert { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion
}