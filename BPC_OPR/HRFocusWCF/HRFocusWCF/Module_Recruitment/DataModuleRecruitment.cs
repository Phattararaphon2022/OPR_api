using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BPC_OPR
{
    public class DataModuleRecruitment
    {
    }


    [DataContract]
    public class InputMTApplywork
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int applywork_id { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        
        [DataMember]
        public string applywork_initial { get; set; }
        [DataMember]
        public string applywork_fname_th { get; set; }
        [DataMember]
        public string applywork_lname_th { get; set; }
        [DataMember]
        public string applywork_fname_en { get; set; }
        [DataMember]
        public string applywork_lname_en { get; set; }

       
        [DataMember]
        public string applywork_birthdate { get; set; }
        [DataMember]
        public string applywork_startdate { get; set; }
       
        [DataMember]
        public string province_code { get; set; }
        [DataMember]
        public string bloodtype_code { get; set; }
        [DataMember]
        public double applywork_height { get; set; }
        [DataMember]
        public double applywork_weight { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    //ReqWorker
    [DataContract]
    public class InputReqWorker
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public int worker_id { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string worker_card { get; set; }
        [DataMember]
        public string worker_initial { get; set; }
        [DataMember]
        public string worker_fname_th { get; set; }
        [DataMember]
        public string worker_lname_th { get; set; }
        [DataMember]
        public string worker_fname_en { get; set; }
        [DataMember]
        public string worker_lname_en { get; set; }

        [DataMember]
        public string worker_type { get; set; }
        [DataMember]
        public string worker_gender { get; set; }
        [DataMember]
        public string worker_birthdate { get; set; }
        [DataMember]
        public string worker_hiredate { get; set; }
        [DataMember]
        public string worker_status { get; set; }
        [DataMember]
        public string religion_code { get; set; }
        [DataMember]
        public string blood_code { get; set; }
        [DataMember]
        public double worker_height { get; set; }
        [DataMember]
        public double worker_weight { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }

        [DataMember]
        public double worker_age { get; set; }
        [DataMember]
        public string worker_tel { get; set; }
        [DataMember]
        public string worker_email { get; set; }
        [DataMember]
        public string worker_line { get; set; }
        [DataMember]
        public string worker_facebook { get; set; }
        [DataMember]
        public string worker_military { get; set; }

        [DataMember]
        public string reqworker_data { get; set; }
    }

    //Fillter All
    [DataContract]
    public class FillterApplywork
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
        public DateTime paydate { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
    }


    [DataContract]
    public class InputApplyTransaction
    {
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string transaction_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputReqWorkerTransaction
    {
        [DataMember]
        public string work_code { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string transaction_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputTRApplyAddress
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string applyaddress_id { get; set; }
        [DataMember]
        public string address_type { get; set; }
        [DataMember]
        public string address_no { get; set; }
        [DataMember]
        public string address_moo { get; set; }
        [DataMember]
        public string address_soi { get; set; }
        [DataMember]
        public string address_road { get; set; }
        [DataMember]
        public string address_tambon { get; set; }
        [DataMember]
        public string address_amphur { get; set; }
        [DataMember]
        public string province_code { get; set; }
        [DataMember]
        public string address_zipcode { get; set; }
        [DataMember]
        public string address_tel { get; set; }
        [DataMember]
        public string address_email { get; set; }
        [DataMember]
        public string address_line { get; set; }
        [DataMember]
        public string address_facebook { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }
    [DataContract]
    public class InputTRApplyCard
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public int card_id { get; set; }
        [DataMember]
        public string card_code { get; set; }
        [DataMember]
        public string card_type { get; set; }
        [DataMember]
        public string card_issue { get; set; }
        [DataMember]
        public string card_expire { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    public class InputTRReqForeigner
    {
        [DataMember]
        public int foreigner_id { get; set; }
        [DataMember]
        public string passport_no { get; set; }
        [DataMember]
        public string passport_issue { get; set; }
        [DataMember]
        public string passport_expire { get; set; }
        [DataMember]
        public string visa_no { get; set; }
        [DataMember]
        public string visa_issue { get; set; }
        [DataMember]
        public string visa_expire { get; set; }
        [DataMember]
        public string workpermit_no { get; set; }
        [DataMember]
        public string workpermit_by { get; set; }
        [DataMember]
        public string workpermit_issue { get; set; }
        [DataMember]
        public string workpermit_expire { get; set; }
        [DataMember]
        public string entry_date { get; set; }
        [DataMember]
        public string certificate_no { get; set; }
        [DataMember]
        public string certificate_expire { get; set; }
        [DataMember]
        public string otherdoc_no { get; set; }
        [DataMember]
        public string otherdoc_expire { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    public class InputTRReqEducation
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int reqeducation_no { get; set; }
        [DataMember]
        public string reqeducation_gpa { get; set; }
        [DataMember]
        public string reqeducation_start { get; set; }
        [DataMember]
        public string reqeducation_finish { get; set; }
        [DataMember]
        public string institute_code { get; set; }
        [DataMember]
        public string faculty_code { get; set; }
        [DataMember]
        public string major_code { get; set; }
        [DataMember]
        public string qualification_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    [DataContract]
    public class InputTRReqAssessment
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        [DataMember]
        public int reqassessment_id { get; set; }
        [DataMember]
        public string reqassessment_location { get; set; }
        [DataMember]
        public string reqassessment_topic { get; set; }
        [DataMember]
        public DateTime reqassessment_fromdate { get; set; }
        [DataMember]
        public DateTime reqassessment_todate { get; set; }
        [DataMember]
        public double reqassessment_count { get; set; }
        [DataMember]
        public string reqassessment_result { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    [DataContract]
    public class InputTRReqCriminal
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        [DataMember]
        public int reqcriminal_id { get; set; }
        [DataMember]
        public string reqcriminal_location { get; set; }
        [DataMember]
        public DateTime reqcriminal_fromdate { get; set; }
        [DataMember]
        public DateTime reqcriminal_todate { get; set; }
        [DataMember]
        public double reqcriminal_count { get; set; }
        [DataMember]
        public string reqcriminal_result { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    public class InputTRReqSuggest
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int reqsuggest_id { get; set; }
        [DataMember]
        public string reqsuggest_code { get; set; }
        [DataMember]
        public string reqsuggest_date { get; set; }
        [DataMember]
        public string reqsuggest_note { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    [DataContract]
    public class InputTRReqTraining
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int reqtraining_no { get; set; }
        [DataMember]
        public DateTime reqtraining_start { get; set; }
        [DataMember]
        public DateTime reqtraining_finish { get; set; }
        [DataMember]
        public string reqtraining_status { get; set; }
        [DataMember]
        public double reqtraining_hours { get; set; }
        [DataMember]
        public double reqtraining_cost { get; set; }
        [DataMember]
        public string reqtraining_note { get; set; }
        [DataMember]
        public string institute_code { get; set; }
        [DataMember]
        public string institute_other { get; set; }
        [DataMember]
        public string course_code { get; set; }
        [DataMember]
        public string course_other { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    //Black List
    [DataContract]
    public class InputBlacklist
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
        public string reason_code { get; set; }
        [DataMember]
        public string blacklist_note { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputDocatt
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string document_id { get; set; }
        

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
        public int status { get; set; }
        //[DataMember]
        //public List<cls_TRReqempinfo> reqempinfo_data { get; set; }
        [DataMember]
        public List<cls_TRDocatt> docatt_data { get; set; }


        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }


    [DataContract]
    public class InputTRApplySalary
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public int empsalary_id { get; set; }
        [DataMember]
        public string empsalary_amount { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }
    [DataContract]
    public class InputTRApplyPosition
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public int empposition_id { get; set; }
        [DataMember]
        public string empposition_position { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }
    [DataContract]
    public class InputTRApplyProject
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public int empproject_id { get; set; }
        [DataMember]
        public string empproject_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }







}