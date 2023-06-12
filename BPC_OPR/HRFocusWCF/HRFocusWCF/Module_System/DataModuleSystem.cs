using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BPC_OPR
{
    public class DataModuleSystem
    {
    }


    [DataContract]
    public class InputMTBank
    {
        [DataMember]
        public string bank_id { get; set; }
        [DataMember]
        public string bank_code { get; set; }
        [DataMember]
        public string bank_name_th { get; set; }
        [DataMember]
        public string bank_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }


    [DataContract]
    public class InputMTCodestructure
    {
        [DataMember]
        public string codestructure_code { get; set; }
        [DataMember]
        public string codestructure_name_th { get; set; }
        [DataMember]
        public string codestructure_name_en { get; set; }
        [DataMember]
        public int index { get; set; }
    }


    public class InputMTPolcode
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int polcode_id { get; set; }
        [DataMember]
        public string polcode_type { get; set; }

        [DataMember]
        public string polcode_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    public class InputTRPolcode
    {
        [DataMember]
        public int polcode_id { get; set; }
        [DataMember]
        public string codestructure_code { get; set; }
        [DataMember]
        public int polcode_lenght { get; set; }
        [DataMember]
        public string polcode_text { get; set; }
        [DataMember]
        public int polcode_order { get; set; }
        [DataMember]
        public int index { get; set; }
    }
    //[DataContract]
    //public class InputSYSReason
    //{
    //    [DataMember]
    //    public string reason_id { get; set; }
    //    [DataMember]
    //    public string reason_code { get; set; }
    //    [DataMember]
    //    public string reason_name_th { get; set; }
    //    [DataMember]
    //    public string reason_name_en { get; set; }
    //    [DataMember]
    //    public string reason_group { get; set; }
    //    [DataMember]
    //    public string created_by { get; set; }
    //    [DataMember]
    //    public string modified_by { get; set; }

    //}


    [DataContract]
    public class FillterCompany
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
        public string created_by { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string combranch_code { get; set; }
        [DataMember]
        public string combranch_id { get; set; }
        [DataMember]
        public string comaddress_type { get; set; }
        [DataMember]
        public string card_type { get; set; }
        [DataMember]
        public string comcard_id { get; set; }
        [DataMember]
        public string comcard_code { get; set; }
        [DataMember]
        public string company_id { get; set; }
         [DataMember]
        public string course_id { get; set; }
         [DataMember]
        public string course_code { get; set; }
        [DataMember]
         public string institute_id { get; set; }
        [DataMember]
         public string institute_code { get; set; }
        [DataMember]
        public string faculty_id { get; set; }
        [DataMember]
        public string faculty_code { get; set; }
        [DataMember]
        public string major_id { get; set; }
        [DataMember]
        public string major_code { get; set; }
        [DataMember]
        public string qualification_id { get; set; }
        [DataMember]
        public string qualification_code { get; set; }
        [DataMember]
        public string comlocation_code { get; set; }
        [DataMember]
        public string comlocation_id { get; set; }
        [DataMember]
        public string polcode_id { get; set; }
        [DataMember]
        public string polcode_type { get; set; }
        [DataMember]
        public string combank_bankaccount { get; set; }
    }

    [DataContract]
    public class InputMTCombranch
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int combranch_id { get; set; }
        [DataMember]
        public string sso_combranch_no { get; set; }
        [DataMember]
        public string combranch_code { get; set; }
        [DataMember]
        public string combranch_name_th { get; set; }
        [DataMember]
        public string combranch_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
    }


    


    [DataContract]
    public class InputMTRounds
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public int rounds_id { get; set; }
        [DataMember]
        public string rounds_code { get; set; }
        [DataMember]
        public string rounds_name_th { get; set; }
        [DataMember]
        public string rounds_name_en { get; set; }
        [DataMember]
        public string rounds_group { get; set; }
        [DataMember]
        public string rounds_data { get; set; }
        [DataMember]
        public string rounds_from { get; set; }
        [DataMember]
        public string rounds_to { get; set; }
        [DataMember]
        public string rounds_result { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputTRRound
    {
        [DataMember]
        public string round_code { get; set; }
        [DataMember]
        public string round_from { get; set; }
        [DataMember]
        public string round_to { get; set; }
        [DataMember]
        public string round_result { get; set; }

        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputMTComLocation
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int comlocation_id { get; set; }
        
        [DataMember]
        public string comlocation_code { get; set; }
        [DataMember]
        public string comlocation_name_th { get; set; }
        [DataMember]
        public string comlocation_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
    }



    [DataContract]
    public class InputMTCompany 
    {
        [DataMember]
        public int company_id { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string company_initials { get; set; }
        [DataMember]
        public string company_name_th { get; set; }
        [DataMember]
        public string company_name_en { get; set; }

        [DataMember]
        public string sso_tax_no { get; set; }
        [DataMember]
        public string citizen_no { get; set; }
        [DataMember]
        public string provident_fund_no { get; set; }



        [DataMember]
        public double hrs_perday { get; set; }

        [DataMember]
        public double sso_com_rate { get; set; }
        [DataMember]
        public double sso_emp_rate { get; set; }
        [DataMember]
        public string sso_security_no { get; set; }
        [DataMember]
        public string sso_branch_no { get; set; }

        [DataMember]
        public double sso_min_wage { get; set; }
        [DataMember]
        public double sso_max_wage { get; set; }
        [DataMember]
        public int sso_min_age { get; set; }
        [DataMember]
        public int sso_max_age { get; set; }

        [DataMember]
        public string card_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
    }



    [DataContract]
    public class InputComTransaction
    {   
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string combranch_code { get; set; }
        [DataMember]
        public string comaddress_type { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string transaction_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string comcard_id { get; set; }
        [DataMember]
        public string course_id { get; set; }
        [DataMember]
        public string institute_id { get; set; }
        [DataMember]
        public string faculty_id { get; set; }
         [DataMember]
        public string major_id { get; set; }
         [DataMember]
         public string qualification_id { get; set; }
         [DataMember]
         public string comlocation_code { get; set; }
         [DataMember]
         public int combank_id { get; set; }
    }



    [DataContract]
    public class InputMTComaddress
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string combranch_code { get; set; }
        [DataMember]
        public string comaddress_type { get; set; }

        [DataMember]
        public string comaddressth_no { get; set; }
        [DataMember]
        public string comaddressth_moo { get; set; }
        [DataMember]
        public string comaddressth_soi { get; set; }
        [DataMember]
        public string comaddressth_road { get; set; }
        [DataMember]
        public string comaddressth_tambon { get; set; }
        [DataMember]
        public string comaddressth_amphur { get; set; }
        [DataMember]
        public string comaddress_zipcode { get; set; }
        [DataMember]
        public string provinceth_code { get; set; }



        [DataMember]
        public string comaddressen_no { get; set; }
        [DataMember]
        public string comaddressen_moo { get; set; }
        [DataMember]
        public string comaddressen_soi { get; set; }
        [DataMember]
        public string comaddressen_road { get; set; }
        [DataMember]
        public string comaddressen_tambon { get; set; }
        [DataMember]
        public string comaddressen_amphur { get; set; }
        
        [DataMember]
        public string provinceen_code { get; set; }


        [DataMember]
        public string comaddress_tel { get; set; }
        [DataMember]
        public string comaddress_email { get; set; }
        [DataMember]
        public string comaddress_line { get; set; }
        [DataMember]
        public string comaddress_facebook { get; set; }
        
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }



    [DataContract]
    public class InputMTComaddlocation
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string comlocation_code { get; set; }
        
        [DataMember]
        public string comaddress_type { get; set; }

        [DataMember]
        public string comaddlocationth_no { get; set; }
        [DataMember]
        public string comaddlocationth_moo { get; set; }
        [DataMember]
        public string comaddlocationth_soi { get; set; }
        [DataMember]
        public string comaddlocationth_road { get; set; }
        [DataMember]
        public string comaddlocationth_tambon { get; set; }
        [DataMember]
        public string comaddlocationth_amphur { get; set; }
        [DataMember]
        public string comaddlocation_zipcode { get; set; }
        [DataMember]
        public string provinceth_code { get; set; }



        [DataMember]
        public string comaddlocationen_no { get; set; }
        [DataMember]
        public string comaddlocationen_moo { get; set; }
        [DataMember]
        public string comaddlocationen_soi { get; set; }
        [DataMember]
        public string comaddlocationen_road { get; set; }
        [DataMember]
        public string comaddlocationen_tambon { get; set; }
        [DataMember]
        public string comaddlocationen_amphur { get; set; }

        [DataMember]
        public string provinceen_code { get; set; }


        [DataMember]
        public string comaddlocation_tel { get; set; }
        [DataMember]
        public string comaddlocation_email { get; set; }
        [DataMember]
        public string comaddlocation_line { get; set; }
        [DataMember]
        public string comaddlocation_facebook { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }



    [DataContract]
    public class InputMTCardtype
    {
        [DataMember]
        public string cardtype_id { get; set; }
        [DataMember]
        public string cardtype_code { get; set; }
        [DataMember]
        public string cardtype_name_th { get; set; }
        [DataMember]
        public string cardtype_name_en { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    [DataContract]
    public class InputMTCombank
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int combank_id { get; set; }
        [DataMember]
        public string combank_bankcode { get; set; }
        [DataMember]
        public string combank_bankaccount { get; set; }       
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }

    }

    [DataContract]
    public class InputMTCombankk
    {
        [DataMember]
        public int combank_id { get; set; }
        [DataMember]
        public string combank_bankcode { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string combank_bankaccount { get; set; }
        [DataMember]
        public string combank_nameaccount { get; set; }

        [DataMember]
        public double combank_bankpercent { get; set; }

        [DataMember]
        public double combank_cashpercent { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    [DataContract]
    public class InputMTComcard
    {
        [DataMember]
        public int comcard_id { get; set; }
        [DataMember]
        public string comcard_code { get; set; }
        [DataMember]
        public string card_type { get; set; }
        [DataMember]
        public string comcard_issue { get; set; }
        [DataMember]
        public string comcard_expire { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string combranch_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputMTCourse
    {
        [DataMember]
        public int course_id { get; set; }
        [DataMember]
        public string course_code { get; set; }
        [DataMember]
        public string course_name_th { get; set; }
        [DataMember]
        public string course_name_en { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputMTInstitute
    {
        [DataMember]
        public int institute_id { get; set; }
        [DataMember]
        public string institute_code { get; set; }
        [DataMember]
        public string institute_name_th { get; set; }
        [DataMember]
        public string institute_name_en { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }


    [DataContract]
    public class InputMTFaculty
    {
        [DataMember]
        public int faculty_id { get; set; }
        [DataMember]
        public string faculty_code { get; set; }
        [DataMember]
        public string faculty_name_th { get; set; }
        [DataMember]
        public string faculty_name_en { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputMTMajor
    {
        [DataMember]
        public int major_id { get; set; }
        [DataMember]
        public string major_code { get; set; }
        [DataMember]
        public string major_name_th { get; set; }
        [DataMember]
        public string major_name_en { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }


    [DataContract]
    public class InputMTQualification
    {
        [DataMember]
        public int qualification_id { get; set; }
        [DataMember]
        public string qualification_code { get; set; }
        [DataMember]
        public string qualification_name_th { get; set; }
        [DataMember]
        public string qualification_name_en { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputMTRequest
    {
        [DataMember]
        public int request_id { get; set; }
        [DataMember]
        public string request_code { get; set; }
        [DataMember]
        public DateTime request_date { get; set; }
        [DataMember]
        public string request_agency { get; set; }
        [DataMember]
        public string request_work { get; set; }
        [DataMember]
        public string request_job_type { get; set; }
        [DataMember]
        public string request_employee_type { get; set; }
        [DataMember]
        public double request_quantity { get; set; }
        [DataMember]
        public string request_urgency { get; set; }
        [DataMember]
        public double request_wage_rate { get; set; }
        [DataMember]
        public double request_overtime { get; set; }
        [DataMember]
        public string request_another { get; set; }

        //[DataMember]
        //public string created_by { get; set; }
        //[DataMember]
        //public DateTime created_date { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    #region InputMTReason
    [DataContract]
    public class InputMTReason
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
        public string reason_id { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public string reason_name_th { get; set; }
        [DataMember]
        public string reason_name_en { get; set; }
        [DataMember]
        public string reason_group { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public bool flag { get; set; }
    }
    #endregion

    #region InputMTLocation
    [DataContract]
    public class InputMTLocation
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
        public string location_id { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string location_name_th { get; set; }
        [DataMember]
        public string location_name_en { get; set; }
        [DataMember]
        public string location_detail { get; set; }
        [DataMember]
        public string location_lat { get; set; }
        [DataMember]
        public string location_long { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }
    #endregion

    #region InputMTYear
    [DataContract]
    public class InputMTYear
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
        public string year_id { get; set; }
        [DataMember]
        public string year_code { get; set; }
        [DataMember]
        public string year_name_th { get; set; }
        [DataMember]
        public string year_name_en { get; set; }
        [DataMember]
        public string year_fromdate { get; set; }
        [DataMember]
        public string year_todate { get; set; }
        [DataMember]
        public string year_group { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }
    #endregion


    [DataContract]
    public class InputMTFamily
    {
        [DataMember]
        public string family_id { get; set; }
        [DataMember]
        public string family_code { get; set; }
        [DataMember]
        public string family_name_th { get; set; }
        [DataMember]
        public string family_name_en { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    [DataContract]
    public class InputMTLevel
    {
        [DataMember]
        public string level_id { get; set; }
        [DataMember]
        public string level_code { get; set; }
        [DataMember]
        public string level_name_th { get; set; }
        [DataMember]
        public string level_name_en { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }



    }

   
    [DataContract]
    public class InputMTReduce
    {
        [DataMember]
        public int reduce_id { get; set; }
        [DataMember]
        public string reduce_code { get; set; }
        [DataMember]
        public string reduce_name_th { get; set; }
        [DataMember]
        public string reduce_name_en { get; set; }

        [DataMember]
        public double reduce_amount { get; set; }
        [DataMember]
        public double reduce_percent { get; set; }
        [DataMember]
        public double reduce_percent_max { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
    }


    [DataContract]
    public class InputMTReligion
    {
        [DataMember]
        public string religion_id { get; set; }
        [DataMember]
        public string religion_code { get; set; }
        [DataMember]
        public string religion_name_th { get; set; }
        [DataMember]
        public string religion_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }



    }

    [DataContract]
    public class InputMTProvince
    {
        [DataMember]
        public string province_id { get; set; }
        [DataMember]
        public string province_code { get; set; }
        [DataMember]
        public string province_name_th { get; set; }
        [DataMember]
        public string province_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }



    }
    [DataContract]
    public class InputMTAddresstype
    {
        [DataMember]
        public string addresstype_id { get; set; }
        [DataMember]
        public string addresstype_code { get; set; }
        [DataMember]
        public string addresstype_name_th { get; set; }
        [DataMember]
        public string addresstype_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }



    }
    [DataContract]
    public class InputMTEthnicity
    {
        [DataMember]
        public string ethnicity_id { get; set; }
        [DataMember]
        public string ethnicity_code { get; set; }
        [DataMember]
        public string ethnicity_name_th { get; set; }
        [DataMember]
        public string ethnicity_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }



    }

    [DataContract]
    public class InputMTBloodtype
    {
        [DataMember]
        public string bloodtype_id { get; set; }
        [DataMember]
        public string bloodtype_code { get; set; }
        [DataMember]
        public string bloodtype_name_th { get; set; }
        [DataMember]
        public string bloodtype_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }




    }
    [DataContract]
    public class InputMTHospital
    {
        [DataMember]
        public string hospital_id { get; set; }
        [DataMember]
        public string hospital_code { get; set; }
        [DataMember]
        public string hospital_name_th { get; set; }
        [DataMember]
        public string hospital_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }
}

 
