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
      

        //-- Transaction
        //[DataMember]
        //public string card_data { get; set; }
        //[DataMember]
        //public string reduce_data { get; set; }
        //[DataMember]
        //public string salary_data { get; set; }
        //[DataMember]
        //public string family_data { get; set; }
        //[DataMember]
        //public string dep_data { get; set; }
        //--

        ////[DataMember]
        ////public bool self_admin { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }

        //[DataMember]
        //public string worker_pwd { get; set; }
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
        public string applywork_code { get; set; }
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
        public string applywork_code { get; set; }
        [DataMember]
        public string applyaddress_id { get; set; }
        [DataMember]
        public string applyaddress_type { get; set; }
        [DataMember]
        public string applyaddress_no { get; set; }
        [DataMember]
        public string applyaddress_moo { get; set; }
        [DataMember]
        public string applyaddress_soi { get; set; }
        [DataMember]
        public string applyaddress_road { get; set; }
        [DataMember]
        public string applyaddress_tambon { get; set; }
        [DataMember]
        public string address_amphur { get; set; }
        [DataMember]
        public string applyprovince_code { get; set; }
        [DataMember]
        public string applyaddress_zipcode { get; set; }
        [DataMember]
        public string applyaddress_tel { get; set; }
        [DataMember]
        public string applyaddress_email { get; set; }
        [DataMember]
        public string applyaddress_line { get; set; }
        [DataMember]
        public string applyaddress_facebook { get; set; }
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

    [DataContract]
    public class InputTRReqTraining
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string applywork_code { get; set; }
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







}